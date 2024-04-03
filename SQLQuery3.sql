select * from ClockIns;

SELECT e.FirstName, e.LastName, ci.ClockInTime, ci.ClockOutTime
FROM Employee e
LEFT JOIN ClockIns ci ON e.Id = ci.EmployeeId
WHERE e.Id = 2;

DELETE FROM ClockIns
WHERE Id = 16;

select * from Employee;

