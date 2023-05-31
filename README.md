API-dokumentation

Denne dokumentation beskriver de tilgængelige endpoints og deres funktionalitet i API'en.

GetAll

Returnerer en liste over alle kunder.

    URL

    /getall

    Metode

    GET

    Respons

    En liste over Customer-objekter.

GetById

Returnerer en kunde baseret på kundens ID.

    URL

    /getbyid/{id}

    Metode

    GET

    Parametre

    id - Kundens ID.

    Respons

    Kundens Customer-objekt.

GetByEmail

Returnerer en kunde baseret på kundens e-mailadresse.

    URL

    /getbyemail/{email}

    Metode

    GET

    Parametre

    email - Kundens e-mailadresse.

    Respons

    Kundens Customer-objekt.

DeleteById

Sletter en kunde baseret på kundens ID.

    URL

    /deletebyid/{id}

    Metode

    DELETE

    Parametre

    id - Kundens ID.

    Respons

    Kundens Customer-objekt.

DeleteByEmail

Sletter en kunde baseret på kundens e-mailadresse.

    URL

    /deletebyemail/{email}

    Metode

    DELETE

    Parametre

    email - Kundens e-mailadresse.

    Respons

    Kundens Customer-objekt.

UpdateCustomer

Opdaterer en kundes oplysninger.

    URL

    /updatecustomer

    Metode

    PUT

    Parametre

    data - Et Customer-objekt indeholdende de opdaterede oplysninger.

    Respons

    Kundens Customer-objekt efter opdateringen.

CreateCustomer

Opretter en ny kunde.

    URL

    /createcustomer

    Metode

    POST

    Parametre

    data - Et Customer-objekt indeholdende de nye kundeoplysninger.

    Respons

    Den oprettede Customer-objekt.

CheckCredentials

Tjekker gyldigheden af brugerens legitimationsoplysninger.

    URL

    /checkcredentials

    Metode

    GET

    Parametre

    data - Et Credentials-objekt indeholdende brugerens e-mailadresse og adgangskode.

    Respons

    En boolsk værdi, der angiver om brugerens legitimationsoplysninger er gyldige.
    
Customer-klasse

Customer-klassen repræsenterer en kunde i systemet.
Egenskaber

    Id (string?): Kundens ID. 
    (Denne egenskab er annoteret med [BsonId] og [BsonRepresentation(BsonType.ObjectId)],
    hvilket indikerer, at den bruger et ObjectId som ID og repræsenterer det som en string.)
    FirstName (string): Kundens fornavn.
    LastName (string): Kundens efternavn.
    Gender (string): Kundens køn.
    BirthDate (DateTime): Kundens fødselsdato.
    Address (string): Kundens adresse.
    PostalCode (string): Kundens postnummer.
    City (string): Kundens by.
    Country (string): Kundens land.
    Telephone (string): Kundens telefonnummer.
    Email (string): Kundens e-mailadresse.
    AccessCode (string): Kundens adgangskode.

Konstruktør

    Customer(string firstName, string lastName, string gender, DateTime birthDate, string address, string postalCode, string city, string country, string telephone, string email, string accessCode):
    Konstruktøren opretter en ny Customer-instans med de nødvendige oplysninger om kunden.
    
    
## Oprettelse af ny kunde - JSON-format

For at oprette en ny kunde ved hjælp af API'en skal du sende følgende JSON-data som en del af din anmodning:

```json
{
    "firstName": "Hanne",
    "lastName": "Gylling",
    "gender": "Kvinde",
    "birthDate": "1998-05-11T16:31:05.768Z",
    "address": "pilkjærsvej 2",
    "postalCode": "8210",
    "city": "Aarhus V",
    "country": "DK",
    "telephone": "12341234",
    "email": "hagy@haha.dk",
    "accessCode": "SCRUMMASTER123"
}

``` 
## Kunden - JSON-format

Når du modtager en kunde som svar fra API'en, vil JSON-dataene have følgende format:

```json
{
    "id": "645e3b620e1f633f43d210c4",
    "firstName": "Henrik",
    "lastName": "Andersen",
    "gender": "M",
    "birthDate": "1980-06-15T00:00:00Z",
    "address": "Hovedgade 123",
    "postalCode": "1234",
    "city": "Aarhus",
    "country": "DK",
    "telephone": "12345678",
    "email": "henrik@example.com",
    "accessCode": "MinKode123"
}
``` 
## CheckCredentials - JSON-format

Når du sender en anmodning til `/checkcredentials` endpoint, skal du inkludere følgende JSON-data som en del af din anmodning:

```json
{
    "email": "henrik@example.com",
    "accessCode": "MinKode123"
}
