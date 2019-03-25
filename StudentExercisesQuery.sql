--drop tables in opposite order of created tables
DROP TABLE IF EXISTS Instructor;
DROP TABLE IF EXISTS StudentExercise;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS Cohort;
DROP TABLE IF EXISTS Exercise;

--create tables, columns, and foreign key restraints
--Create tables from each entity in the Student Exercises ERD.

CREATE TABLE Exercise (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	ExerciseName VARCHAR(55) NOT NULL,
	ExerciseLanguage VARCHAR(55) NOT NULL
);

CREATE TABLE Cohort (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	CohortName VARCHAR(55) NOT NULL
);

CREATE TABLE Student (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	SlackHandle VARCHAR(55) NOT NULL,
	CohortId INTEGER NOT NULL,
	CONSTRAINT FK_Student_Cohort_Id FOREIGN KEY(CohortId) REFERENCES Cohort(Id)
);

CREATE TABLE StudentExercise ( 
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	StudentId INTEGER NOT NULL,
	ExerciseId INTEGER NOT NULL,
	CONSTRAINT FK_Student_Id FOREIGN KEY(StudentId) REFERENCES Student(Id),
	CONSTRAINT FK_Exercise_Id FOREIGN KEY(ExerciseId) REFERENCES Exercise(Id)
);

CREATE TABLE Instructor (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	SlackHandle VARCHAR(55) NOT NULL,
	CohortId INTEGER NOT NULL,
	CONSTRAINT FK_Instructor_Cohort_Id FOREIGN KEY(CohortId) REFERENCES Cohort(Id)
);

--use insert statements to create data in the tables
--You should have 2-3 cohorts, 5-10 students, 4-8 instructors, 2-5 exercises and each student should be assigned 1-2 exercises.

INSERT INTO Exercise (ExerciseName, ExerciseLanguage) VALUES ('LINQ Intro', 'CSharp');
INSERT INTO Exercise (ExerciseName, ExerciseLanguage) VALUES ('Array Methods', 'JavaScript');
INSERT INTO Exercise (ExerciseName, ExerciseLanguage) VALUES ('My First Webpage', 'HTML');
INSERT INTO Exercise (ExerciseName, ExerciseLanguage) VALUES ('Flexbox', 'CSS');
INSERT INTO Exercise (ExerciseName, ExerciseLanguage) VALUES ('Dictionaries and Lists', 'CSharp');

INSERT INTO Cohort (CohortName) VALUES ('C29');
INSERT INTO Cohort (CohortName) VALUES ('C30');
INSERT INTO Cohort (CohortName) VALUES ('E1');

INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Beyonce', 'Knowles', '@QueenBee', 1);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Kanye', 'West', '@YzySzn', 2);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('James', 'Blake', '@JBlake', 3);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Carly Rae', 'Jepsen', '@PartyForOne', 1);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Earl', 'Sweatshirt', '@OddFuture1', 2);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Donald', 'Glover', '@ChildishGambino', 3);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Todd', 'Terje', '@ItsAlbumTime', 1);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Annie', 'Clark', '@StVincent', 2);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Ariana', 'Grande', '@ThankUNext', 3);
INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) VALUES ('Janelle', 'Monae', '@DjangoJane', 1);

INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) VALUES ('Eric', 'Andre', '@EAndre', 1);
INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) VALUES ('Nathan', 'Fielder', '@Nathan4U', 2);
INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) VALUES ('Ilana', 'Glazer', '@IGlazer', 3);
INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) VALUES ('Karen', 'Kilgariff', '@KarenK', 1);

INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (1, 2);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (1, 1);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (2, 3);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (3, 4);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (3, 5);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (4, 2);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (5, 1);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (5, 3);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (6, 5);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (7, 5);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (7, 4);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (8, 1);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (9, 3);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (9, 2);
INSERT INTO StudentExercise(StudentId, ExerciseId) VALUES (10, 5);


