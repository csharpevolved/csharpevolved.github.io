# 2026-06-17 — Home snippet horizontal overflow regression validation

- Extended `scripts/check-var-feature.mjs` with deterministic CSS contract assertions for `.home-feature-snippet-grid` container bounds (`width: 100%` + `max-width: 100%`).
- Added assertions for `.home-feature-snippet-grid .feature-snippet-card` width constraints (`width: 100%`, `max-width: 100%`, `min-width: 0`).
- Added forbidden-rule guards to reject conflicting home-snippet grid/card declarations (`width: 100vw`, non-auto `margin-inline`, `margin-left`/`margin-right`, and `transform` usage).
- Kept checks lightweight and inside the existing `npm run build` → `check:var` workflow.
