# Copilot Debugging & Refactoring Log

## Issue 1: Missing Validation for Invalid Emails
- **Problem:** Invalid email strings like "invalid-email" were accepted by the `POST /api/users` endpoint.
- **Copilot Prompt:** "How do I trigger DataAnnotation validation inside a Minimal API POST handler in ASP.NET Core?"
- **Resolution:** Copilot suggested using `Validator.TryValidateObject` to evaluate `CreateUserDto` properties and return a `400 Bad Request` with validation error messages.

## Issue 2: Duplicate ID Generation on Empty List
- **Problem:** Attempting to create a user when the list was empty caused an exception on `users.Max(u => u.Id)`.
- **Copilot Prompt:** "Fix SequenceContainsNoElements exception when computing max ID on an empty list."
- **Resolution:** Copilot refactored the ID assignment logic to check `users.Count > 0 ? users.Max(u => u.Id) + 1 : 1`.

## Issue 3: Middleware Execution Order
- **Problem:** Request duration logs printed `0ms` because stopwatch tracking was placed after `await _next(context)`.
- **Copilot Prompt:** "Why is my ASP.NET Core request timing middleware reporting 0ms duration?"
- **Resolution:** Copilot corrected the middleware delegate order by starting `Stopwatch.StartNew()` before `await _next(context)` and stopping it after `_next` finished.