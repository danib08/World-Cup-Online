CREATE TABLE dbo.Tournament(
ID int IDENTITY(1,1) NOT NULL,
Name varchar(30) NOT NULL,
StartDate datetime NOT NULL,
EndDate datetime NOT NULL,
Description varchar(1000),
TypeID int NOT NULL
)

CREATE TABLE dbo.Phase(
ID int IDENTITY(1,1) NOT NULL,
Name varchar(50) NOT NULL,
TournamentID int NOT NULL,
)

CREATE TABLE dbo.Team(
ID varchar(8) NOT NULL,
Name varchar(30) NOT NULL,
Confederation varchar(30) NOT NULL,
TypeID int NOT NULL
)

CREATE TABLE dbo.Player(
ID varchar(15) NOT NULL,
Name varchar(30) NOT NULL,
Lastname varchar(30) NOT NULL,
Position varchar(30) NOT NULL
)

CREATE TABLE dbo.Match(
ID int IDENTITY(1,1) NOT NULL,
StartDate datetime NOT NULL,
StartTime time NOT NULL,
Score varchar(7) NOT NULL,
Location varchar(50) NOT NULL,
StateID int NOT NULL,
TournamentID int NOT NULL,
PhaseID int NOT NULL
)

CREATE TABLE dbo.State(
ID int IDENTITY(1,1) NOT NULL,
Name varchar(30) NOT NULL
)

CREATE TABLE dbo.Type(
ID int IDENTITY(1,1) NOT NULL,
Name varchar(30) NOT NULL
)

CREATE TABLE dbo.Team_In_Tournament(
TeamID varchar(8) NOT NULL,
TournamentID int NOT NULL
)

CREATE TABLE dbo.Player_In_Team(
PlayerID varchar(15) NOT NULL,
TeamID varchar(8) NOT NULL,
JerseyNum INT NOT NULL
)

CREATE TABLE dbo.Team_In_Match(
TeamID varchar(8) NOT NULL,
MatchID int NOT NULL
)

---------------------------------------------------------------------------------------

ALTER TABLE dbo.Tournament
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.Phase
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.Team
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.Player
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.Match
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.State
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.Type
ADD PRIMARY KEY (ID)

ALTER TABLE dbo.Team_In_Tournament
ADD CONSTRAINT PK_TeamTourn PRIMARY KEY(TeamID, TournamentID)

ALTER TABLE dbo.Player_In_Team
ADD CONSTRAINT PK_PlayerTeam PRIMARY KEY(PlayerID, TeamID)

ALTER TABLE dbo.Team_In_Match
ADD CONSTRAINT PK_TeamMatch PRIMARY KEY(TeamID, MatchID)

--------------------------------------------------------------------------------------

ALTER TABLE dbo.Tournament
ADD CONSTRAINT FK_Tourn FOREIGN KEY(TypeID)
REFERENCES dbo.Type(ID)

ALTER TABLE dbo.Phase
ADD CONSTRAINT FK_Phase FOREIGN KEY(TournamentID)
REFERENCES dbo.Tournament(ID)

ALTER TABLE dbo.Team
ADD CONSTRAINT FK_Team FOREIGN KEY(TypeID)
REFERENCES dbo.Type(ID)

ALTER TABLE dbo.Match
ADD CONSTRAINT FK_State FOREIGN KEY(StateID)
REFERENCES dbo.State(ID)

ALTER TABLE dbo.Match
ADD CONSTRAINT FK_Match_TournID FOREIGN KEY(TournamentID)
REFERENCES dbo.Tournament(ID)

ALTER TABLE dbo.Match
ADD CONSTRAINT FK_Match_PhaseID FOREIGN KEY(PhaseID)
REFERENCES dbo.Phase(ID)

ALTER TABLE dbo.Team_In_Tournament
ADD CONSTRAINT FK_TIT_TeamID FOREIGN KEY(TeamID)
REFERENCES dbo.Team(ID)

ALTER TABLE dbo.Team_In_Tournament
ADD CONSTRAINT FK_TIT_TournID FOREIGN KEY(TournamentID)
REFERENCES dbo.Tournament(ID)

ALTER TABLE dbo.Player_In_Team
ADD CONSTRAINT FK_PIT_PlayerID FOREIGN KEY(PlayerID)
REFERENCES dbo.Player(ID)

ALTER TABLE dbo.Player_In_Team
ADD CONSTRAINT FK_PIT_TeamID FOREIGN KEY(TeamID)
REFERENCES dbo.Team(ID)

ALTER TABLE dbo.Team_In_Match
ADD CONSTRAINT FK_TIM_Team FOREIGN KEY(TeamID)
REFERENCES dbo.Team(ID)

ALTER TABLE dbo.Team_In_Match
ADD CONSTRAINT FK_TIM_Match FOREIGN KEY(MatchID)
REFERENCES dbo.Match(ID)
