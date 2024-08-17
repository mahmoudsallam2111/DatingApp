import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Lightbox } from 'ngx-lightbox';
import { ToastrService } from 'ngx-toastr';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account.service';
import { AuthUser } from '../../_models/authUser';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs') memberTabs?: TabsetComponent;

  galleryImages: any[];

  member: User;
  messages: Message[];

  activeTab: TabDirective;
  user?: AuthUser;
  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _memberService: MembersService,
    private _lightbox: Lightbox,
    private _toastre: ToastrService,
    private _messagesService: MessageService,
    public _PresenceService: PresenceService,
    private _accountService: AccountService
  ) {
    this._accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.user = user;
      },
    });
  }
  ngOnDestroy(): void {
    this._messagesService.stopHubConnection();
  }

  ngOnInit(): void {
    this.loadMember();

    this._route.queryParamMap.subscribe({
      next: (params) => {
        params['tab'] && this.selectTab(params['tab']);
      },
    });
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const messageTab = this.memberTabs.tabs.find(
        (x) => x.heading === heading
      );
      if (messageTab) messageTab.active = true;
    }
  }
  loadMember() {
    const username = this._route.snapshot.paramMap.get('username');
    if (!username) {
      return;
    }
    this._memberService.getMember(username).subscribe({
      next: (member) => {
        this.member = member;
        this.galleryImages = this.getImages();
      },
    });
  }

  getImages() {
    if (!this.member || !this.member.photos) {
      return [];
    }
    return this.member.photos.map((photo) => ({
      src: photo.url,
      thumb: photo.url,
    }));
  }

  open(index: number): void {
    this._lightbox.open(this.galleryImages, index);
  }

  close(): void {
    this._lightbox.close();
  }

  addLike(member: User) {
    this._memberService.addLike(member.userName).subscribe({
      next: (_) => {
        this._toastre.success('you have liked ' + member.knownAs);
      },
      error: (error) => {
        this._toastre.error(error.error.detail);
      },
    });
  }

  loadmessages() {
    if (this.member.userName) {
      this._messagesService
        .getMessagesThread(this.member.userName)
        .subscribe((response) => {
          this.messages = response;
        });
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    this._router.navigate([], {
      relativeTo: this._route,
      queryParams: { tab: this.activeTab.heading },
      queryParamsHandling: 'merge',
    });

    if (this.activeTab.heading === 'Messages' && this.user) {
      this._messagesService.createHubConnection(
        this.user,
        this.member.userName
      );
    } else {
      this._messagesService.stopHubConnection();
    }
  }
}
