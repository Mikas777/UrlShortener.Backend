using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addtextintopageabout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SiteContents",
                columns: new[] { "Key", "Value" },
                values: new object[] { "AboutPageContent", "Our URL Shortener uses a robust algorithm to create short, unique identifiers for your long URLs. Here's how it works:\r\n\r\n1.  Check for Existing URL: When you submit a long URL, we first check if it has already been shortened in our system. If it exists, we return the existing short code to avoid duplicates.\r\n2.  Generate Random Code: If the URL is new, we generate a random string of a fixed length (e.g., 7 characters) using a Base62 alphabet (a-z, A-Z, 0-9). This provides a vast number of possible combinations. We use a cryptographically secure random number generator for better randomness.\r\n3.  Check for Uniqueness: Although highly unlikely with Base62 and sufficient length, we check if the newly generated short code already exists in our database.\r\n4.  Retry if Collision: In the rare event of a collision (the generated code is already in use), we repeat step 2 and 3 until a unique code is found.\r\n5.  Store: Once a unique short code is generated, we store the original long URL and its corresponding short code, along with creation details, in our database.\r\n6.  Return: The unique short code is then returned to you.\r\n\r\nWhen someone visits the short URL, our server looks up the short code in the database and permanently redirects (301) the user to the original long URL." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SiteContents",
                keyColumn: "Key",
                keyValue: "AboutPageContent");
        }
    }
}
