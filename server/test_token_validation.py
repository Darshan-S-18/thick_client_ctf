from __future__ import annotations

import base64
import datetime as dt
import unittest

from token_validation import (
    EXPECTED_PATCHED_METHOD_HASH,
    decode_token,
    is_fresh,
    validate_token,
)


def make_token(method_hash: str, timestamp: str, ui_state: str, runtime_value: str) -> str:
    payload = f"{method_hash}|{timestamp}|{ui_state}|{runtime_value}"
    return base64.b64encode(payload.encode("utf-8")).decode("utf-8")


class TokenValidationTests(unittest.TestCase):
    def test_decode_token_happy_path(self):
        token = make_token(
            "0" * 64,
            "202604090500",
            "110",
            "A" * 24,
        )
        parts = decode_token(token)
        self.assertEqual(parts.ui_state, "110")

    def test_decode_token_rejects_bad_format(self):
        broken = base64.b64encode(b"not|enough|fields").decode("utf-8")
        with self.assertRaises(ValueError):
            decode_token(broken)

    def test_is_fresh_with_fixed_now(self):
        now = dt.datetime(2026, 4, 9, 5, 0, 0)
        self.assertTrue(is_fresh("202604090500", now=now))
        self.assertFalse(is_fresh("202604090458", now=now))

    def test_validate_token_rejects_wrong_hash(self):
        token = make_token("1" * 64, "202604090500", "110", "B" * 24)
        parts = decode_token(token)
        valid, reason = validate_token(parts, expected_hash=EXPECTED_PATCHED_METHOD_HASH)
        self.assertFalse(valid)
        self.assertIn("wrong method hash", reason)


if __name__ == "__main__":
    unittest.main()
