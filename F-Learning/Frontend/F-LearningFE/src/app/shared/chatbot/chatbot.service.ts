import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GeminiService {
  private apiKey = 'AIzaSyBjNRJnAXM44Vg1JctGGzgIdwc4kHik8DY'; // Thay bằng API key từ Google AI Studio
  private apiUrl = 'https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent';

  async getChatResponse(message: string): Promise<string> {
    try {
      const response = await fetch(`${this.apiUrl}?key=${this.apiKey}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          contents: [
            {
              parts: [
                {
                  text: message,
                },
              ],
            },
          ],
        }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error.message || 'Lỗi khi gọi API Gemini.');
      }

      const data = await response.json();
      return data.candidates[0].content.parts[0].text.trim();
    } catch (error: any) {
      throw new Error(error.message || 'Không thể kết nối tới Gemini.');
    }
  }
}