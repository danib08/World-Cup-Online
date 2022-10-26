exec procedure proc_tournament(@ID varchar(6),
				@Name varchar(30),
				@StartDate datetime,
				@EndDate datetime,
				@Description varchar(1000),
				@TypeID int,
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Tournament(ID,Name,StartDate,EndDate,Description,TypeID)
		values(@ID,@Name,@StartDate,@EndDate,@Description,@TypeID)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Tournament
	end

	if @StatementType = 'Select WebApp'
	begin
		select t.ID, t.name, StartDate, EndDate, Description, Type.Name as Type
		from Tournament as t join Type on TypeID = Type.ID
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Tournament
		where ID = @ID
	end

	if @StatementType = 'Select One WebApp'
	begin
		select t.ID, t.name, StartDate, EndDate, Description, Type.Name as Type
		from Tournament as t join Type on TypeID = Type.ID
		where t.ID = @ID
	end

	if @StatementType = 'Update'
	begin
		update dbo.Tournament set Name=@Name,StartDate=@StartDate,EndDate=@EndDate,Description=@Description,TypeID=@TypeID
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Tournament
		where ID = @ID
	end

	if @StatementType = 'Get Matches by Tourn'
	begin	
		select dbo.Match.ID, dbo.Team.Name, StartDate, StartTime, Location, dbo.State.Name as State, Score
		from ((dbo.Match join dbo.State on StateID = dbo.State.ID) join dbo.Team_In_Match on MatchID = dbo.Match.ID) join dbo.Team on TeamID = dbo.Team.ID
		where TournamentID = @ID
		order by dbo.Match.ID 
	end

	if @StatementType = 'Get Phases by Tourn'
	begin	
		select ID as value, Name as label
		from Phase
		where TournamentID = @ID
	end

	if @StatementType = 'Get Teams by Tourn'
	begin	
		select ID, Name, Confederation
		from (Team_In_Tournament join Team on TeamID = ID)
		where TournamentID = @ID
	end

end
go


exec procedure proc_team(@ID varchar(8),
			   	@Name varchar(30),
				@Confederation varchar(30),
				@TypeID int,
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Team(ID,Name,Confederation,TypeID)
		values(@ID,@Name,@Confederation,@TypeID)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Team
	end

	if @StatementType = 'Select WebApp'
	begin
		select ID, Name as label
		from dbo.Team
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Team
		where ID = @ID
	end

	if @StatementType = 'Select Type'
	begin
		select * from dbo.Team
		where TypeID = @TypeID
	end

	if @StatementType = 'Update'
	begin
		update dbo.Team set Name=@Name,Confederation=@Confederation,TypeID=@TypeID
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Team
		where ID = @ID
	end
end
go


create procedure proc_teamInTournament(@TeamID varchar(8),
				@TournamentID varchar(6),
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Team_In_Tournament(TeamID,TournamentID)
		values(@TeamID,@TournamentID)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Team_In_Tournament
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Team_In_Tournament
		where TeamID = @TeamID and TournamentID = @TournamentID
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Team_In_Tournament
		where TeamID = @TeamID and TournamentID = @TournamentID
	end
end
go

create procedure proc_player(@ID varchar(15),
				@Name varchar(30),
				@Lastname varchar(30),
				@Position varchar(30),
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Player(ID,Name,Lastname,Position)
		values(@ID,@Name,@Lastname,@Position)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Player
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Player
		where ID = @ID
	end

	if @StatementType = 'Update'
	begin
		update dbo.Player set Name=@Name,Lastname=@Lastname,Position=@Position
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Player
		where ID = @ID
	end
end
go


create procedure proc_player_In_Team(@TeamID varchar(6),
				@PlayerID varchar(15),
				@JerseyNum int,
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Player_In_Team(TeamID,PlayerID,JerseyNum)
		values(@TeamID,@PlayerID,@JerseyNum)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Player_In_Team
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Player_In_Team
		where TeamID = @TeamID and PlayerID= @PlayerID
	end


	if @StatementType = 'Update'
	begin
		update dbo.Player_In_Team set JerseyNum=@JerseyNum
		where TeamID = @TeamID and PlayerID= @PlayerID	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Player_In_Team 
		where TeamID = @TeamID and PlayerID= @PlayerID
	end
end
go


exec procedure proc_phase(@ID int,
				@Name varchar(50),
				@TournamentID varchar(6),
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Phase(Name,TournamentID)
		values(@Name,@TournamentID)
	end

	if @StatementType = 'Select'
	begin
		select ID as value, Name as label from dbo.Phase
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Phase
		where ID = @ID
	end


	if @StatementType = 'Update'
	begin
		update dbo.Phase set Name=@Name, TournamentID=@TournamentID
		where ID = @ID	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Phase 
		where ID = @ID
	end
end
go

create procedure proc_match(@ID int,
			    @StartDate datetime,
			    @StartTime time,
			    @Score varchar(7),
			    @Location varchar(50),
			    @StateID int,
			    @TournamentID varchar(6),
				@PhaseID int,
			    @StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Match(StartDate,StartTime,Score,Location,StateID,TournamentID,PhaseID)
		values(@StartDate,@StartTime,@Score,@Location,@StateID,@TournamentID,@PhaseID)
		select SCOPE_IDENTITY() as ID
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Match
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Match
		where ID = @ID
	end

	if @StatementType = 'Update'
	begin
		update dbo.Match set StartDate=@StartDate,StartTime=@StartTime,Location=@Location,StateID=@StateID,TournamentID=@TournamentID,PhaseID=@PhaseID
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Match 
		where ID = @ID
	end
end
go


create procedure proc_state(@ID int,
				@Name varchar(30),
			    @StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.State(Name)
		values(@Name)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.State
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.State
		where ID = @ID
	end

	if @StatementType = 'Update'
	begin
		update dbo.State set Name=@Name
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.State
		where ID = @ID
	end
end
go

create procedure proc_teamInMatch(@TeamID varchar(8),
				@MatchID int,
			    @StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Team_In_Match(TeamID,MatchID)
		values(@TeamID,@MatchID)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Team_In_Match
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Team_In_Match
		where TeamID = @TeamID and MatchID = @MatchID
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Team_In_Match
		where TeamID = @TeamID and MatchID = @MatchID
	end
end
go

create procedure proc_type(@ID int,
				@Name varchar(30),
			    @StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Type(Name)
		values(@Name)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Type
	end

	if @StatementType = 'Select WebApp'
	begin
		select ID as value, Name as label
		from dbo.Type
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.State
		where ID = @ID
	end

	if @StatementType = 'Update'
	begin
		update dbo.Type set Name=@Name
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Type
		where ID = @ID
	end
end
go
