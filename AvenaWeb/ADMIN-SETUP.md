# Avena admin publishing setup

1. Apply `AvenaWeb-Database-Admin-v1.sql` to the existing PostgreSQL/Supabase database before deploying the app.
2. Register the account that will manage news, then promote that existing account in the database console by replacing `your_username` below with its exact username:

   ```sql
   UPDATE "User"
   SET "IsAdmin" = TRUE
   WHERE LOWER("Username") = LOWER('your_username');
   ```

3. Confirm the update matched one account, sign out and back in so the admin role is added to the authentication cookie, then open `/Admin/News`.

Registration never grants admin access. Only accounts explicitly promoted in the database receive the `Admin` role.
