import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements OnInit {
  @Input({ required: true }) userName: string;
  @Input({
    required: true,
  })
  messages: Message[];

  constructor(private _messagesService: MessageService) {}
  ngOnInit(): void {}
}
