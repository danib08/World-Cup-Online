CREATE TABLE dbo.Tournament(
ID varchar(6) NOT NULL,
Name varchar(30) NOT NULL,
StartDate datetime NOT NULL,
EndDate datetime NOT NULL,
Local bit NOT NULL,
Description varchar(1000) NOT NULL
)

CREATE TABLE dbo.Phase(
ID INT NOT NULL,
Name varchar(50) NOT NULL,
TournamentID varchar(6) NOT NULL,
)

CREATE TABLE dbo.Team(
ID varchar(8) NOT NULL,
Name varchar(30) NOT NULL,
Confederation varchar(30) NOT NULL,
Local bit NOT NULL
)

CREATE TABLE dbo.Player(
ID varchar(15) NOT NULL,
Name varchar(30) NOT NULL,
LastName varchar(30) NOT NULL,
Position varchar(30) NOT NULL
)

CREATE TABLE dbo.Team_In_Tournament(
TeamID varchar(8) NOT NULL,
TournamentID varchar(6) NOT NULL
)

CREATE TABLE dbo.Player_In_Team(
PlayerID varchar(15) NOT NULL,
TeamID varchar(8) NOT NULL,
JerseyNum INT NOT NULL
)
