import {
  ChangeDetectorRef,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input({ required: true })
  userName: string;
  messageContent: string = '';

  constructor(
    public _messagesService: MessageService,
    private cdr: ChangeDetectorRef
  ) {}
  ngOnInit(): void {
    this._messagesService.messageThread$.subscribe(() => {
      this.cdr.detectChanges();
    });
  }

  sendMessage() {
    if (!this.userName) return;

    this._messagesService
      .sendMessage(this.userName, this.messageContent)
      .then(() => {
        this.messageForm?.reset();
      });
  }
}
