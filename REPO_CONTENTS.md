# Repository Contents (Visibility Guide)

If your UI previously showed paths like `a/None` in the diff viewer, use this file as a stable index of the actual checked-in files and locations.

## Top-level
- `README.md`
- `REPO_CONTENTS.md`
- `client/`
- `server/`
- `docs/`

## Client (.NET Framework WinForms)
- `client/ThickClientCTF.sln`
- `client/ThickClientCTF/ThickClientCTF.csproj`
- `client/ThickClientCTF/Program.cs`
- `client/ThickClientCTF/MainForm.cs`
- `client/ThickClientCTF/MainForm.Designer.cs`
- `client/ThickClientCTF/TokenBuilder.cs`
- `client/ThickClientCTF/App.config`
- `client/ThickClientCTF/Properties/AssemblyInfo.cs`

## Server (Flask)
- `server/app.py`
- `server/token_validation.py`
- `server/test_token_validation.py`
- `server/requirements.txt`

## Docs
- `docs/CHALLENGE.md`
- `docs/ORGANIZER_SOLUTION.md`

## Quick verification commands
```bash
git ls-tree --name-only -r HEAD | sort
find . -maxdepth 3 -type f | sort
```
