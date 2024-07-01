CREATE DATABASE student;
use student;

CREATE TABLE students(
    StudentID int not null AUTO_INCREMENT,
    FirstName varchar(100) Not null,
    Surname varchar(100) Not Null, 
    PRIMARY KEY (StudentID)
);

INSERT INTO students (FirstName, Surname)
VALUES ("John", "Anderson"),("Emma","Smith");