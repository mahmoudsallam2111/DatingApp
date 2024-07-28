import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css',
})
export class MessagesComponent implements OnInit {
  messages?: Message[];
  pagination?: Pagination;
  container: string;
  pageNumber: number = 1;
  pageSize: number = 5;

  constructor(private _messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this._messageService
      .getMessages(this.pageNumber, this.pageNumber, this.container)
      .subscribe((response) => {
        (this.messages = response.result),
          (this.pagination = response.pagination);
      });
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }
}
