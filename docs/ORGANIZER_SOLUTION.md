# Organizer Solution Path

## Intended Solve Route
1. Open client in dnSpy/ILSpy.
2. Locate `MainForm.CheckAccess()` and patch body to return `true`.
3. Optionally inspect `MainForm_KeyDown` to find reveal sequence (Up, Up, Down, Down, Left, Right, Left, Right, B, A).
4. Run patched binary.
5. Trigger reveal sequence and click `Request Secure Flag`.
6. Client computes token as base64 of:
   - SHA256(IL bytes of `CheckAccess`)
   - UTC timestamp (`yyyyMMddHHmm`)
   - UI bitfield (`revealed`, `clicked`, `decoy`)
   - Runtime hash material
7. Client requests `GET /flag?token=...`.
8. Server validates token and returns flag.

## Setup Notes
- Update server constant `EXPECTED_PATCHED_METHOD_HASH` to match your patched binary build.
- One easy method: add temporary logging in client to display generated method hash after patching.

## Why It Works
- Static-only analysis is insufficient; valid method hash is tied to executed patched IL.
- Replay is blocked by strict timestamp freshness.
- Direct scripted API usage requires reconstructing runtime/UI constraints correctly.
