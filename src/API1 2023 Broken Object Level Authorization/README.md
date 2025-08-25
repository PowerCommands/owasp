# API1:2023 Broken Object Level Authorization

Denna sårbarhet uppstår när ett API inte kontrollerar att den autentiserade användaren har behörighet att läsa eller ändra det objekt som begärs.
Angripare kan manipulera objektidentifierare (t.ex. användar-ID) i URL:er eller parametrar för att komma åt andra användares data.

## Exempel

Detta exempel innehåller ett API som returnerar användarprofil baserat på ett `id`. API:t verifierar inte att anroparen har rätt att hämta uppgifterna.
Genom att ändra parametern kan en angripare få åtkomst till andra användares uppgifter.
