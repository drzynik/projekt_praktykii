using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praktyki_backend.Models;
using System.Globalization;
using System.IO;
using System.Text;

namespace praktyki_backend.Controllers
{
    [ApiController]
    [Route("api/admin/deck")]
    public class PdfImportController : ControllerBase
    {
        private readonly dbcontext _context;

        public PdfImportController(dbcontext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] string email, [FromForm] string username, [FromForm] string password)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Names == username && u.Password == password);

            if (existingUser != null)
            {
                return Ok(new { userId = existingUser.Users_Id });
            }

            var newUser = new Users
            {
                Names = username,
                Email = email,
                Password = password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok(new { userId = newUser.Users_Id });
        }

        [HttpPost("upload")]
        public IActionResult UploadFile(IFormFile file, [FromForm] int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Users_Id == userId);
            if (user == null)
                return BadRequest("Niepoprawny userId");

            if (file == null || file.Length == 0)
                return BadRequest("Nie wysłano pliku");

            // 1. Zapis pliku tymczasowo
            var tempPath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(tempPath))
            {
                file.CopyTo(stream);
            }

            // 2. Odczyt PDF jako tekst
            string pdfText;
            using (var reader = new iText.Kernel.Pdf.PdfReader(tempPath))
            {
                var pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader);
                var sb = new StringBuilder();
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    var page = pdfDoc.GetPage(i);
                    sb.Append(iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page));
                }
                pdfText = sb.ToString();
            }

            // 3. Usuń plik tymczasowy
            System.IO.File.Delete(tempPath);

            //do mapowania cards id krok uno
            var cardIdsFromPdf = new HashSet<int>();

            void CollectCardIds(string section)
            {
                var rows = section.Split("//");
                foreach (var row in rows)
                {
                    if (string.IsNullOrWhiteSpace(row)) continue;
                    var cols = row.Split(';');
                    if (int.TryParse(cols[0], out int cardId))
                        cardIdsFromPdf.Add(cardId);
                }
            }
            CollectCardIds(GetSection(pdfText, "#Decisions"));
            CollectCardIds(GetSection(pdfText, "#Hardwares"));
            CollectCardIds(GetSection(pdfText, "#Softwares"));
            CollectCardIds(GetSection(pdfText, "#Feedbacks"));
            CollectCardIds(GetSection(pdfText, "#CardsWeight"));
            CollectCardIds(GetSection(pdfText, "#CardsEnablers"));

            // 4. Tutaj parsujemy pdfText do modeli i zapisujemy do bazy
            // Przykład dla użytkownika + deck:
            //var user = new Users { Names = username, Email = email, Password = password };
            //_context.Users.Add(user);
            //_context.SaveChanges();
            //var userid = user.Users_Id;

            var deck = new Decks { Users_Id = userId, Deck_Name = file.FileName };
            _context.Decks.Add(deck);
            _context.SaveChanges();
            var deckid = deck.Decks_Id;

            var cards = cardIdsFromPdf
                .Select(pdfCardId => new Cards
                {
                    Decks_Id = deckid,
                    Card_Id = pdfCardId
                })
                .ToList();

            _context.Cards.AddRange(cards);
            _context.SaveChanges();

            var cardIdMap = cards.ToDictionary(
                c => c.Card_Id,    // ID z PDF
                c => c.Cards_Id    // ID z bazy
            );

            //decisions
            var decisionsSection = GetSection(pdfText, "#Decisions");
            var decisionsRows = decisionsSection.Split("//");
            foreach (var row in decisionsRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;
                var cols = row.Split(';');
                if (cols.Length < 5)
                    continue;
                if (!int.TryParse(cols[0], out int pdfCardId))
                    continue;
                //zabezpieczenie
                if (!cardIdMap.ContainsKey(pdfCardId))
                    throw new Exception($"Decisions: brak karty {pdfCardId}");

                int.TryParse(cols[3], out int costBits);
                int.TryParse(cols[4], out int costBitsWeight);
                var decision = new Decisions
                {
                    Cards_Id = cardIdMap[pdfCardId],
                    Decisions_Short_Desc = cols[1],
                    Decisions_Long_Desc = cols[2],
                    Decisions_Cost_Bits = costBits,
                    Decisions_Cost_Bits_Weight = costBitsWeight,
                    Decks_Id = deckid
                };
                _context.Decisions.Add(decision);
            }
            _context.SaveChanges();

            //hardwares
            var hardwaresSection = GetSection(pdfText, "#Hardwares");
            var hardwaresRows = hardwaresSection.Split("//");
            foreach (var row in hardwaresRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;
                var cols = row.Split(';');
                if (cols.Length < 5)
                    continue;
                if (!int.TryParse(cols[0], out int pdfCardId))
                    continue;
                //zabezpieczenie
                if (!cardIdMap.ContainsKey(pdfCardId))
                    throw new Exception($"Hardwares: brak karty {pdfCardId}");

                double.TryParse(cols[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double costBits);
                int.TryParse(cols[4], out int costBitsWeight);

                var hardwares = new Hardwares
                {
                    Cards_Id = cardIdMap[pdfCardId],
                    Hardwares_Short_Desc = cols[1],
                    Hardwares_Long_Desc = cols[2],
                    Hardwares_Cost_Bits = costBits,
                    Hardwares_Cost_Bits_Weight = costBitsWeight,
                    Decks_Id = deckid
                };
                _context.Hardwares.Add(hardwares);
            }
            _context.SaveChanges();

            //softwares
            var softwaresSection = GetSection(pdfText, "#Softwares");
            var softwaresRows = softwaresSection.Split("//");
            foreach (var row in softwaresRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;
                var cols = row.Split(';');
                if (cols.Length < 5)
                    continue;
                if (!int.TryParse(cols[0], out int pdfCardId))
                    continue;
                //zabezpieczenie
                if (!cardIdMap.ContainsKey(pdfCardId))
                    throw new Exception($"Softwares: brak karty {pdfCardId}");
                
                int.TryParse(cols[3], out int costBits);
                int.TryParse(cols[4], out int costBitsWeight);

                var softwares = new Softwares
                {
                    Cards_Id = cardIdMap[pdfCardId],
                    Softwares_Short_Desc = cols[1],
                    Softwares_Long_Desc = cols[2],
                    Softwares_Cost_Bits = costBits,
                    Softwares_Cost_Bits_Weight = costBitsWeight,
                    Decks_Id = deckid
                };
                _context.Softwares.Add(softwares); 
            }
            _context.SaveChanges();

            //feedbacks
            var feedbacksSection = GetSection(pdfText, "#Feedbacks");
            var feedbacksRows = feedbacksSection.Split("//");
            foreach (var row in feedbacksRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;
                var cols = row.Split(';');
                if (!int.TryParse(cols[0], out int pdfCardId))
                    continue;
                if (!cardIdMap.ContainsKey(pdfCardId))
                    throw new Exception($"Feedbacks: brak karty {pdfCardId}");

                string? cellValue = (cols.Length > 1) ? cols[1] : null;
                bool status = false;
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    if (bool.TryParse(cellValue, out bool result))
                        status = result;
                }
                string? longDesc = (cols.Length > 2 && !string.IsNullOrWhiteSpace(cols[2])) ? cols[2] : null;

                var feedbacks = new Feedbacks
                {
                    Cards_Id = cardIdMap[pdfCardId],
                    Status = status,
                    Feedbacks_Long_Description = longDesc,
                    Decks_Id = deckid
                };
                _context.Feedbacks.Add(feedbacks);
            }
            _context.SaveChanges();

            //processes
            var processesSection = GetSection(pdfText, "#Processes");
            var processesRows = processesSection.Split("//");
var processMap = new Dictionary<int, int>(); // pdfId -> DB PK

            foreach (var row in processesRows)
            {
                if (string.IsNullOrWhiteSpace(row)) continue;

                var cols = row.Split(';');
                if (cols.Length < 5) continue;

                int pdfProcessId = int.Parse(cols[0]);
                var shortDesc = cols[1];
                var longDesc = cols[2];
                var color = cols[3];
                var weightStr = cols[4].Replace(',', '.');
                double weight = double.Parse(weightStr, CultureInfo.InvariantCulture);

                var process = new Processes
                {
                    Processes_Desc = shortDesc,
                    Processes_Desc_Long = longDesc,
                    Processes_Color = color,
                    Processes_Weight = weight,
                    Decks_Id = deckid
                };

                _context.Processes.Add(process);
                _context.SaveChanges(); // MUSI BYĆ TU

                processMap[pdfProcessId] = process.Processes_Id;
            }

            // gameevents
            var eventsSection = GetSection(pdfText, "#GameEvents");
            var eventsRows = eventsSection.Split("//");
            foreach (var row in eventsRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;

                var cols = row.Split(';');

                var shortDesc = (cols.Length > 0) ? cols[0] : null;
                var longDesc = (cols.Length > 1) ? cols[1] : null;

                int turns = 0;
                if (cols.Length > 2) int.TryParse(cols[2], out turns);

                double? decisionsWeights = (cols.Length > 3 && double.TryParse(cols[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double dw)) ? dw : (double?)null;
                double? hardwaresWeights = (cols.Length > 4 && double.TryParse(cols[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double hw)) ? hw : (double?)null;
                double? softwaresWeights = (cols.Length > 5 && double.TryParse(cols[5], NumberStyles.Any, CultureInfo.InvariantCulture, out double sw)) ? sw : (double?)null;
                double? boost_x = (cols.Length > 6 && double.TryParse(cols[6], NumberStyles.Any, CultureInfo.InvariantCulture, out double bx)) ? bx : (double?)null;
                double? boost_y = (cols.Length > 7 && double.TryParse(cols[7], NumberStyles.Any, CultureInfo.InvariantCulture, out double by)) ? by : (double?)null;

                var gameevents = new Gameevents
                {
                    Events_Short_Desc = shortDesc,
                    Events_Long_Desc = longDesc,
                    Turns_Time = turns,
                    Decisions_Cost_Bits_Weight = decisionsWeights,
                    Hardware_Cost_Bits_Weight = hardwaresWeights,
                    Software_Cost_Bits_Weight = softwaresWeights,
                    Boosters_X = boost_x,
                    Boosters_Y = boost_y,
                    Decks_Id = deckid
                };

                _context.Gameevents.Add(gameevents);
            }
            _context.SaveChanges();

            // cardweights
            var cardweightSection = GetSection(pdfText, "#CardsWeight");
            var cardweightRows = cardweightSection.Split("//");

            foreach (var row in cardweightRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;
                var cols = row.Split(';');
                if (cols.Length < 2)
                    continue;
                // PDF card ID
                if (!int.TryParse(cols[0], out int pdfCardId))
                    continue;
                if (!cardIdMap.ContainsKey(pdfCardId))
                    throw new Exception($"Cardweights: brak karty {pdfCardId}");
                // PDF process ID
                if (!int.TryParse(cols[1], out int pdfProcessId))
                    continue;
                if (!processMap.ContainsKey(pdfProcessId))
                    throw new Exception($"Cardweights: brak procesu {pdfProcessId}");
                int? weight_x = null;
                if (cols.Length > 2 && int.TryParse(cols[2], out int wx))
                    weight_x = wx;
                int? weight_y = null;
                if (cols.Length > 3 && int.TryParse(cols[3], out int wy))
                    weight_y = wy;

                bool processExists = _context.Processes
                .Any(p => p.Processes_Id == processMap[pdfProcessId]);

                if (!processExists)
                    throw new Exception($"Proces {pdfProcessId} (DB id {processMap[pdfProcessId]}) NIE ISTNIEJE w bazie");

                var cardweight = new Cardweights
                {
                    Cards_Id = cardIdMap[pdfCardId],
                    Processes_Id = processMap[pdfProcessId],
                    Weights_X = weight_x,
                    Weights_Y = weight_y
                };
                _context.Cardweights.Add(cardweight);
            }
            _context.SaveChanges();

            //cardenablers
            var cardenablerSection = GetSection(pdfText, "#CardsEnablers");
            var cardenablerRows = cardenablerSection.Split("//");
            foreach (var row in cardenablerRows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;
                var cols = row.Split(';');
                if (!int.TryParse(cols[0], out int pdfCardId))
                    continue;
                //zabezpieczenie
                if (!cardIdMap.ContainsKey(pdfCardId))
                    throw new Exception($"Cardenablers: brak karty {pdfCardId}");

                int.TryParse(cols[1], out int enablerId);
                var cardenabler = new Cardenablers
                {
                    Cards_Id = cardIdMap[pdfCardId],
                    Enablers_Id = enablerId
                };
                _context.Cardenablers.Add(cardenabler);
            }
            _context.SaveChanges();

            var lastHardware = _context.Hardwares
                .OrderByDescending(h => h.Hardwares_Id)  // lub inny PK/kolumna daty
                .FirstOrDefault();

            var card = _context.Cards
                .FirstOrDefault(c => c.Cards_Id == lastHardware.Cards_Id);

            int lastPdfId = card.Card_Id;
            //var lastDecisions = _context.Decisions
            //.Where(d => d.Decks_Id == deckid)
            //.OrderByDescending(d => d.Decisions_Id) // zakładam, że Decisions_Id jest PK
            //.Take(5) // np. ostatnie 5 decyzji
            //.Select(d => d.Decisions_Short_Desc)
            //.ToList();
            return Ok(new
            {
                UserID = userId,
                DeckId = deckid,
                //LastDecisions = lastDecisions
                LastHardwareId = lastPdfId,
            });
        }
        string GetSection(string text, string sectionName)
        {
            int start = text.IndexOf(sectionName);
            if (start == -1)
                return string.Empty;

            start += sectionName.Length;

            int nextSection = text.IndexOf("\n#", start);
            if (nextSection == -1)
                nextSection = text.Length;

            return text.Substring(start, nextSection - start).Trim();
        }

        [HttpGet("decisions-by-deck")]
        public IActionResult GetDecisionsByDeck([FromQuery] string deckName)
        {


            if (string.IsNullOrWhiteSpace(deckName))
                return BadRequest("Brak deckName");



            // 1. Pobierz deck po nazwie
            var decks = _context.Decks.ToList();

            var deck = decks.FirstOrDefault(d =>
                Path.GetFileNameWithoutExtension(d.Deck_Name)
                    .Equals(Path.GetFileNameWithoutExtension(deckName), StringComparison.OrdinalIgnoreCase)
            );


            if (deck == null)
                return NotFound("Nie znaleziono decku o podanej nazwie");

            int deckId = deck.Decks_Id;

            // 2. Pobierz decyzje powiązane z deckId
            var decisions = _context.Decisions
                .Where(d => d.Decks_Id == deckId)
                .Join(
                    _context.Cards,
                    dec => dec.Cards_Id,
                    card => card.Cards_Id,
                    (dec, card) => new
                    {
                        CardId = card.Card_Id,
                        ShortDesc = dec.Decisions_Short_Desc,
                        LongDesc = dec.Decisions_Long_Desc
                    }
                )
                .ToList();

            return Ok(decisions);
        }

    }
}