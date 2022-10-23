create procedure proc_tournament(@ID varchar(6),
				@Name varchar(30),
				@StartDate datetime,
				@EndDate datetime,
				@Local bit,
				@Description varchar(1000),
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Tournament(ID,Name,StartDate,EndDate,Local,Description)
		values(@ID,@Name,@StartDate,@EndDate,@Local,@Description)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Tournament
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Tournament
		where Id = @Id
	end


	if @StatementType = 'Update'
	begin
		update dbo.Tournament set Name=@Name,StartDate=@StartDate,EndDate=@EndDate,Local=@Local,Description=@Description
		where ID=@ID 	
	end

	if @StatementType = 'Delete'
	begin
		delete from dbo.Tournament
		where ID = @ID
	end
end
go



create procedure proc_team(@ID varchar(8),
			   @Name varchar(30),
				@Confederation varchar(30),
				@Local bit,
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Team(ID,Name,Confederation,Local)
		values(@ID,@Name,@Confederation,@Local)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Team
	end

	if @StatementType = 'Select One'
	begin
		select * from dbo.Team
		where ID = @ID
	end


	if @StatementType = 'Update'
	begin
		update dbo.Team set Name=@Name,Confederation=@Confederation,Local=@Local
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

/**
	if @StatementType = 'Update'
	begin
		update dbo.Team set Name=@Name,Confederation=@Confederation,Local=@Local
		where ID=@ID 	
	end
*/
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


create procedure proc_phase(@ID int,
				@Name varchar(50),
				@TournamentID varchar(6),
				@StatementType varchar(50) = '')
as begin

	if @StatementType = 'Insert'
	begin
		insert into dbo.Phase(ID,Name,TournamentID)
		values(@ID,@Name,@TournamentID)
	end

	if @StatementType = 'Select'
	begin
		select * from dbo.Phase
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