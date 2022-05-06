CREATE TABLE "Motorbike" (
	"Id"	integer,
	"Name"	varchar(40),
	"Price"	integer,
	"Description"	TEXT,
	"Image"	varchar(200),
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "User" (
	"Id"	integer,
	"FirstName"	varchar(15),
	"LastName"	varchar(15),
	"Email"	varchar(20),
	"Password"	varchar(20),
	"Role"	varchar(10),
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "Orders" (
	"Id"	integer,
	"UserID"	integer,
	"Product"	varchar(40),
	"Service"	varchar(20),
	"Price"	real,
	"Status"	integer,
	FOREIGN KEY("UserID") REFERENCES "User"("Id"),
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "Reservations" (
	"Id"	integer,
	"UserID"	integer,
	"MotorbikeID"	integer,
	"From_Date"	Date,
	"To_Date"	Date,
	"Status"	varchar(10),
	FOREIGN KEY("MotorbikeID") REFERENCES "Motorbike"("Id"),
	FOREIGN KEY("UserID") REFERENCES "User"("Id"),
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "Services" (
	"Id"	integer,
	"UserID"	integer,
	"Brand"	varchar(20),
	"Model"	varchar(20),
	"Year"	integer,
	"SPZ"	varchar(20),
	"Shop"	varchar(50),
	"Description"	TEXT,
	"Status"	varchar(10),
	FOREIGN KEY("UserID") REFERENCES "User"("Id"),
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "BranchOffice" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"City"	TEXT NOT NULL,
	"Address"	TEXT NOT NULL,
	PRIMARY KEY("Id" AUTOINCREMENT)
);

INSERT INTO User (FirstName, LastName, Email, Password, Date_of_birth, Role) VALUES ('Karel', 'Gott', 'admin@email.cz', 'admin', '2000-01-01', 'admin');
INSERT INTO User (FirstName, LastName, Email, Password, Date_of_birth, Role) VALUES ('Lucie', 'Bíla', 'user@email.cz', 'user', '2000-01-01', 'user');

INSERT INTO Motorbike (Name, Price, Description, Image) VALUES ('Ducati 350 Desmo ', 3000, 'The Desmo created Ducati history in 1968 and that’s why you’ve voted in as one of the top 80 motorcycles ever. The first desmodromic production motorcycle ever created by Ducati whch produced just 22bhp meaning the machine was light and playful. Since then Ducati have produced a whole range of motorcycles, 250, 450 and off-roaders from the Desmo!', 'https://www.devittinsurance.com/wp-content/uploads/2016/02/Ducati-350-Desmo-credit-Topspeed.jpg');
INSERT INTO Motorbike (Name, Price, Description, Image) VALUES ('BMW R69S', 3500, 'A 1960 special by BMW, the classic boxer twin which a true motorcycling gem of today. BMW added a few luxurious touches to the R69S including Earls fork and a steering damper. If you want to get yourself BMW classic today then you’re looking at a hefty five figure sum, but the machines are in decent riding condition!', 'https://www.devittinsurance.com/wp-content/uploads/2016/02/BMW-R69S-credit-bwmdean.jpg');
INSERT INTO Motorbike (Name, Price, Description, Image) VALUES ('Kawasaki W800', 4000, 'Kawasaki’s W800 sure does give Triumphs Bonneville a run for its money with its retro roadster feel, the old school up-right position isn’t the norm for Kawasaki but they’ve pulled it off. W800 is fitted with a 773cc fuel injected engine so you’ll still have the speed and the classic style.', 'https://www.devittinsurance.com/wp-content/uploads/2016/02/Kawasaki-W800-credit-total-motorcycle.jpg');

INSERT into BranchOffice(City, Address) VALUES("Ostrava", "Svinov");
INSERT into BranchOffice(City, Address) VALUES("Praha", "Modřany");
INSERT into BranchOffice(City, Address) VALUES("Brno", "Hlavní");