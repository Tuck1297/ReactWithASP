# React + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh


# Notes

Still need pages for the following account api routes

4. design a simple homepage that like v0 with their AI
5. Connect account endpoints

6. If I have time come back to do 2 factor authentication

## For database management and connection
- need to add form for adding a database connection
- Path for accessing data
    -> see all accessible databases
    -> select a database
    -> see all database tables
    -> select a database table
    -> first need total number of rows in database
        -> this will inform access to first 50 rows in database
        -> if there are more than 50 rows then create a manage state that will select and retrieve current index of rows in database (organize by pages) - this will all be managed in the front end so the data will not be stored anywhere...

3. watch cloud native video tomorrow (MUST DO!!!)
4. work with ChatGPT AI API before december end of day december 2nd...


## Notes
- This project currently assumes that the accessed postgresql database is under the public schema
- This project currently only deletes, updates and creates rows with simple data types (int, string, double)
- This project currently only works with postgresql databases
- This project can easily integrate other databases which was the intention of the project
- This project deletes tables from schemas but does not delete schemas from a database

## Directions to set up project
1. Fork Repository
Download Nuget packages
2. use PGAdmin and create following databases HackathonDB, SupplyChain, WebsiteInfo
3. Migrate test data to databases created with PGAdmin
4. update connection strings in appsettings.json with infomration to created databases
5. migrate data
NOTE: also point out which areas one could change to get this to work with sql server
6. Can either register for user and add own connection string data on form OR use first test 
7. user account created in DataContext to access test databases
8. talk about features of implementation and current limitations