--This file will set up your database before testing

DELETE FROM project_employee;
DELETE FROM employee;
DELETE FROM department;
DELETE FROM project;
--UPDATE employee SET department_id = NULL;

--Delete all of the data (empty the tables)

--Insert a fake Department
INSERT INTO department VALUES ('dummy dept');

--Insert a fake Employee
INSERT INTO employee VALUES (1, 'bilbo', 'baggins', 'hobbit', '2006-12-12', '2020-01-01');

--Insert a fake Project
INSERT INTO project VALUES ('project y', '2007-11-14', '2019-03-05');

--Assign the employee to the project
INSERT INTO project_employee VALUES (1, 1);



