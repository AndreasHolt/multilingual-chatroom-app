# Real-time "Multilingual" Chatroom App
**Intended usage**: User either joins a room with a room ID, or creates a new room. The user selects their language of preference prior to joining. When a user sends a message in their language in a room, it is received by other users in their own prefered language - allowing them to communicate through language barriers.

## Demo
![Demo](demo.gif)

## Features
- A chatroom application featuring real-time messaging with fast language translation powered by Groq's API
  - Use LLM's such as Llama 3.1 8b, Mixtral 8x7b and Gemma 7b
- Crossplatform: Windows, OSX, Linux and Web (WebAssembly
- Utilizes Avalonia UI for the frontend, and .NET Core for the backend
- Retro Windows 9.x UI because it's fun

## LLMs for Translation vs Traditional Translation APIs
Pros:
- While translation APIs can provide direct word-for-word translations, they may miss contextual meaning and informal language that's common in chat messages. For example, slang and abbreviations
- Better handles conversational flow and tone

Cons:
- Slower than dedicated translation APIs (especially for larger models) and higher computational cost
- May hallucinate

