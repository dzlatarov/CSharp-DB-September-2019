ALTER TABLE Users
ADD CONSTRAINT CHK_PasswordLenght
CHECK (LEN(Password) >= 5)