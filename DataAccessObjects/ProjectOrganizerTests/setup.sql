--This file will set up your database before testing


--Delete all of the data (empty the tables)
DELETE FROM project_employee;
DELETE FROM employee;
DELETE FROM department;
DELETE FROM project;

--Insert a fake Department
SET IDENTITY_INSERT department ON;
INSERT INTO department (department_id, name) VALUES (1, 'dummy dept');
SET IDENTITY_INSERT department OFF;

--Insert a fake Employee
SET IDENTITY_INSERT employee ON;
INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date) VALUES (1, 1, 'bilbo', 'baggins', 'hobbit', '2006-12-12', '2020-01-01');
INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date) VALUES (2, 1, 'gandalf', 'gray', 'wizard', '1001-02-16', '1950-03-07');
SET IDENTITY_INSERT employee OFF;

--Insert a fake Project
SET IDENTITY_INSERT project ON;
INSERT INTO project (project_id, name, from_date, to_date) VALUES (1, 'project y', '2007-11-14', '2019-03-05');
SET IDENTITY_INSERT project OFF;

----Assign the employee to the project
INSERT INTO project_employee VALUES (1, 1);



