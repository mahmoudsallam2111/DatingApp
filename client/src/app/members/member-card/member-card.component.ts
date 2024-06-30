import { Component, Input, ViewEncapsulation } from '@angular/core';
import { User } from '../../_models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
})
export class MemberCardComponent {
  @Input({
    required: true,
  })
  member: User;
}
