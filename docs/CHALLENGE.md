# Advanced Thick Client CTF — Challenge Brief

## Story
A legacy .NET desktop utility claims to keep its “secure flag retrieval flow” protected.
You need to recover the real flag from the remote service.

## Player-Facing Goal
Retrieve the real flag from the API.

## Provided Files
- `ThickClientCTF.exe` (WinForms client)
- API host/port information

## Expected Player Actions
1. Reverse the client and discover hidden UI behavior.
2. Reveal and click the hidden button.
3. Patch gate logic (`CheckAccess`) so execution can proceed.
4. Run the patched client and trigger the protected flow.
5. Let the client generate a valid runtime token and request `/flag`.

## Anti-shortcut Notes
- No plain-text real flag is stored in client binary.
- API checks method IL hash associated with patched client state.
- API checks current UTC minute to resist replay.
- Token includes runtime + UI-dependent state and changes per run.

## Difficulty
Intermediate (30–90 minutes).
