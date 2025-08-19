using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PuppeteerSharp_Demo.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("generate-html-pdf")]
        public async Task<IActionResult> GenerateHtmlPdf()
        {
            // Downloads Chromium if it's not already available
            await new BrowserFetcher().DownloadAsync();

            // Launch headless Chromium
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            // Open a new page
            await using var page = await browser.NewPageAsync();

            // Navigate to a URL (can also be a local MVC view)
            var url = "https://www.google.com/";
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);

            // Generate PDF
            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20px",
                    Bottom = "20px",
                    Left = "20px",
                    Right = "20px"
                }
            });

            // Close the browser
            await browser.CloseAsync();

            // Returns downloadable file
            var fileName = $"PDF_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        [HttpPost]
        [Route("generate-invoice-pdf")]
        public async Task<IActionResult> GenerateInvoicePdf()
        {
            // Downloads Chromium if it's not already available
            await new BrowserFetcher().DownloadAsync();

            // Launch headless Chromium
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            // Open a new page
            await using var page = await browser.NewPageAsync();

            // Navigate to a URL (can also be a local MVC view)
            var url = $"{Request.Scheme}://{Request.Host}/pdf";
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);

            // Generate PDF
            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20px",
                    Bottom = "20px",
                    Left = "20px",
                    Right = "20px"
                }
            });

            // Close the browser
            await browser.CloseAsync();

            // Returns downloadable file
            var fileName = $"PDF_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
