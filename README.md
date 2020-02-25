# The State of Observability in .NET - Tracing .NET applications in production
Sample code and slides

[Link to the event](https://www.meetup.com/MicrosoftDeveloperGraz/events/268748545/)

Traces, metrics, and logs from a production system are extremely useful in order to find and fix bugs in our applications, especially in today's micro-service environments. But collecting and correlating this data is not trivial. Observability offers the solution to this problem.

In this 2-part session, we will look at how you can create an observability system for your .NET applications and how you can extend it in order to trace any part of your code.

Part 1: Tools and general overview
- What is Observability?
- Observability Tools in practice (demo with sample apps): Application Insights, Elastic, Jaeger.
- What is distributed tracing, how does it work in .NET?
- OpenTelemetry.

Part 2: Looking behind the scenes
- How is data captured in a .NET Application? DiagnosticSource, Activity, EventSource.
- Instrumenting a library and collecting traces from production (demo with code).
