
# 📝 NotesAPI

CRUD API using .NET 7.0 Web API. The User can do CRUD Operations, but only if it logged in.
The application uses Identity and Cookie Session Authentication (can also use JWT Token) to validate User.



## ⚠ Cybersecurity Failures

This is a simple app, not dedicated or created to use in **Production**. This is just for testing and educational purposes.

- _Do not use 'sa' SQL Server User (this is like using root in MySQL). Must need create a brand-new user, with restricted permissions (in schemas and objects)._
- _Do not hardcode the Token (this must be written in the settings file)._
- _Do not manipulate Models Context inside in HTTP Methods (a new class must be created, returning the Result to the function)._



## ⚙ Services Used

- .NET Framework Core 7.0 (Web API)
- SQL Server (with Entity Framework ORM)


## 📸Screenshots


### Setting up Insomnia Environment
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/setting_up_enviroment_url.png?raw=true)

### Checking Database (User & Notes Table + EF Migration History)
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/database_config.png?raw=true)

### Heartbeating the Application
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/heartbeat_application.png?raw=true)

### Checking Unauthorized Endpoints
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/unathorized_access.png?raw=true)

### Guests can create a new account (Unique per E-mail)
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/create_user.png?raw=true)

### Now checking Login Endpoint
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/login_test.png?raw=true)

### Now the User, authenticated, can access their notes
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/authorized_access.png?raw=true)

### Checking Database (If User was created and some notes were added)
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/final_database.png?raw=true)

### Logging out
![Setting up the Enviroment Variables in Insomnia](https://github.com/BitNers/NotesAPI/blob/master/blob/master/logout_test.png?raw=true)


## License

[MIT](https://choosealicense.com/licenses/mit/)
