import { Component, Input, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../environments/environment';
import { AuthUser } from '../../_models/authUser';
import { AccountService } from '../../_services/account.service';
import { take } from 'rxjs';
import { Photo } from '../../_models/photo';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit {
  @Input({ required: true }) member: User;

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;

  user: AuthUser;
  constructor(
    private _accountService: AccountService,
    private _memberService: MembersService
  ) {
    this._accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      if (user) {
        this.user = user;
      }
    });
  }
  ngOnInit(): void {
    this.initializeFileUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }
  fileOverAnother(e: any) {}

  setMainPhoto(photo: Photo) {
    this._memberService.setMainPhoto(photo.id).subscribe(() => {
      if (this.member && this.user) {
        this.user.photoUrl = photo.url;
        this._accountService.setcurrentUser(this.user); //to notify other components that depending on the user that the user is updated
        this.member.photoUrl = photo.url;
        this.member.photos.forEach((p) => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
      }
    });
  }

  deleteUserPhoto(photo: Photo) {
    this._memberService.deletePhoto(photo.id).subscribe(() => {
      if (this.member && this.user) {
        this.member.photos = this.member.photos.filter((p) => p.id != photo.id);
      }
    });
  }
  initializeFileUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'User/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headrer) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member?.photos.push(photo);
        if (!photo.isMain && this.member && this.user) {
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this._accountService.setcurrentUser(this.user);
        }
      }
    };
  }
}
