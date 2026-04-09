from __future__ import annotations

import base64
import datetime as dt
from dataclasses import dataclass

# Replace with IL hash from the patched CheckAccess() method after finalizing binary.
EXPECTED_PATCHED_METHOD_HASH = "d6e27f8f0f4d4f7f6afef3fd8fd22a4ca8d64f4d0f5066bc4445de8ed0e2c7e6"
FLAG = "FLAG{runtime_patch_execution_required}"


@dataclass
class TokenParts:
    method_hash: str
    timestamp: str
    ui_state: str
    runtime_value: str


def decode_token(raw_token: str) -> TokenParts:
    try:
        decoded = base64.b64decode(raw_token).decode("utf-8")
    except Exception as exc:
        raise ValueError(f"invalid base64 token: {exc}")

    chunks = decoded.split("|")
    if len(chunks) != 4:
        raise ValueError("token format must contain 4 fields")

    method_hash, timestamp, ui_state, runtime_value = chunks
    if len(method_hash) != 64 or any(c not in "0123456789abcdef" for c in method_hash):
        raise ValueError("method hash is invalid")
    if len(timestamp) != 12 or not timestamp.isdigit():
        raise ValueError("timestamp format must be yyyyMMddHHmm")
    if len(ui_state) != 3 or any(c not in "01" for c in ui_state):
        raise ValueError("ui_state must be a 3-bit string")
    if len(runtime_value) < 24:
        raise ValueError("runtime value too short")

    return TokenParts(method_hash, timestamp, ui_state, runtime_value)


def is_fresh(timestamp: str, allowed_skew_minutes: int = 1, now: dt.datetime | None = None) -> bool:
    submitted = dt.datetime.strptime(timestamp, "%Y%m%d%H%M")
    reference = (now or dt.datetime.utcnow()).replace(second=0, microsecond=0)
    delta = abs((reference - submitted).total_seconds())
    return delta <= allowed_skew_minutes * 60


def validate_token(parts: TokenParts, expected_hash: str = EXPECTED_PATCHED_METHOD_HASH) -> tuple[bool, str]:
    if parts.method_hash != expected_hash:
        return False, "wrong method hash (did you patch CheckAccess?)"

    if not is_fresh(parts.timestamp):
        return False, "stale token (replay blocked)"

    # ui_state[0] = revealed, ui_state[1] = clicked, ui_state[2] = decoy mode.
    if parts.ui_state[:2] != "11":
        return False, "UI path incomplete"

    return True, "ok"
