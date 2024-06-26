import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  galleryImages: any[];

  member: User;

  constructor(
    private _route: ActivatedRoute,
    private _memberService: MembersService,
    private _lightbox: Lightbox
  ) {}

  ngOnInit(): void {
    this.loadMember();
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
}
