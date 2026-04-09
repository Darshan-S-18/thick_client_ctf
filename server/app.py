from __future__ import annotations

from flask import Flask, jsonify, request

from token_validation import FLAG, decode_token, validate_token

app = Flask(__name__)


@app.get("/flag")
def get_flag():
    token = request.args.get("token", "")
    if not token:
        return jsonify({"ok": False, "error": "missing token"}), 400

    try:
        parts = decode_token(token)
    except ValueError as exc:
        return jsonify({"ok": False, "error": str(exc)}), 400

    valid, reason = validate_token(parts)
    if not valid:
        return jsonify({"ok": False, "error": reason}), 403

    return jsonify({"ok": True, "flag": FLAG})


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
