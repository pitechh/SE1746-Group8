import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GeminiService } from './chatbot.service';

@Component({
  selector: 'app-chatbot-box',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chatbot.component.html',
})
export class ChatbotBoxComponent {
  isOpen = true;
  userInput = '';
  messages: { text: string; isUser: boolean }[] = [
    { text: 'Xin chào! Tôi là Trợ lý học tập, có thể giúp gì cho bạn?', isUser: false },
  ];

  constructor(private geminiService: GeminiService) {} // Inject GeminiService

  toggleChat() {
    this.isOpen = !this.isOpen;
  }

  async sendMessage() {
    if (!this.userInput.trim()) return;

    this.messages.push({ text: this.userInput, isUser: true });
    const userMessage = this.userInput;
    this.userInput = '';

    try {
      const response = await this.geminiService.getChatResponse(userMessage);
      this.messages.push({ text: response, isUser: false });
    } catch (error: any) {
      this.messages.push({ text: error.message, isUser: false });
    }
  }
}