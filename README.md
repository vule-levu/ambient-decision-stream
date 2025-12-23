# ambient-decision-stream

This project explores how small, frequent decisions form meaning only when observed over time.

Instead of reacting immediately to every change, the system listens to decision events, tolerates duplication, and emits signals only when patterns stabilize or drift becomes apparent.

There are no dashboards, no synchronous APIs, and no authoritative "current state".
Only events, memory, and delayed interpretation.

## What this is not
- Not a CRUD application
- Not a real-time notification system
- Not a rules engine
- Not an analytics pipeline

## How it works
- Decision changes are emitted as events
- Events may arrive more than once
- Consumers decide *when* something is worth reacting to
- Most of the logic lives in how events are ignored

If you want to understand it, read the code.
