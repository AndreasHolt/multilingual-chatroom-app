# Real-time "Multilingual" Chatroom App

**Motivation**: While traditional translation APIs perform word-for-word translations, they often miss the context and nuances of casual conversations, struggling with slang, sayings, and everyday expressions. This project explores using LLMs for more natural and context-aware translations, enabling people to chat naturally across language barriers in real-time.



## Demo
![Demo](demo.gif)

**Intended usage**: User either joins a room with a room ID, or creates a new room. The user selects their language of preference prior to joining. When a user sends a message in their selected language, it is received by all other users in the room, in their own preferred language - allowing them to communicate through language barriers.

## Features
- A chatroom application featuring real-time messaging (WebSocket-based) with language translation using LLMs (fast inference via Groq's API)
  - Use LLM's such as Llama 3.1 8b, Mixtral 8x7b and Gemma 7b
- Crossplatform: Windows, OSX, Linux and Web (WebAssembly)
- Utilizes Avalonia UI for the frontend, and .NET Core for the backend
- Retro Windows 9.x UI because it's fun

## LLMs for Translation vs Traditional Translation APIs
Pros:
- While translation APIs can provide direct word-for-word translations, they may miss contextual meaning and informal language that's common in chat messages. For example, slang and abbreviations
- Better handles conversational flow and tone

Cons:
- Slower than dedicated translation APIs (especially for larger models) and higher computational cost
- May hallucinate

