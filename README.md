# Advanced Thick Client CTF (Execution-Bound)

This repository contains a .NET Framework WinForms challenge and a Flask flag server designed to enforce a **patch-and-run** solve path.

## Repository Layout
- `client/ThickClientCTF`: WinForms thick client project (.NET Framework 4.8)
- `server/app.py`: Flask API for `GET /flag?token=...`
- `server/token_validation.py`: framework-agnostic token validation logic
- `server/test_token_validation.py`: unit tests for token parsing/validation
- `docs/CHALLENGE.md`: player-facing challenge statement
- `docs/ORGANIZER_SOLUTION.md`: organizer-only expected path

## Build & Run

### 1) Start the server
```bash
cd server
python3 -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
python app.py
```

Optional local validation (no Flask runtime required):
```bash
python3 -m unittest -v test_token_validation.py
```

### 2) Build the WinForms client (Windows host with .NET Framework tools)
```powershell
cd client\ThickClientCTF
msbuild .\ThickClientCTF.csproj /p:Configuration=Release
```

Binary output:
- `client/ThickClientCTF/bin/Release/ThickClientCTF.exe`

## Token Design
The client sends base64-encoded payload:

`method_hash|timestamp|ui_state|runtime_value`

Where:
- `method_hash`: SHA256 over IL bytes of `MainForm.CheckAccess`
- `timestamp`: UTC minute precision (`yyyyMMddHHmm`)
- `ui_state`: bitstring (`revealed`, `clicked`, `decoy`)
- `runtime_value`: runtime-derived hash material (random seed + environment)

## Validation Logic
Server validates:
- structure + decoding
- patched method hash
- freshness window (anti replay)
- required UI states (`revealed=1`, `clicked=1`)

## Important Organizer Step
Set `EXPECTED_PATCHED_METHOD_HASH` in `server/app.py` to match the hash produced by your patched challenge binary before release.

## Notes
- Includes fake flag/decoy path in client.
- Uses reflection and split logic to reduce direct call graph clarity without heavy obfuscation.

## Visibility Note
If your repository UI shows unusual diff paths, use `REPO_CONTENTS.md` as a canonical index of all tracked challenge files.
