import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { LightboxModule } from 'ngx-lightbox';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    LightboxModule,
    NgxSpinnerModule.forRoot({ type: 'line-spin-fade' }),
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    LightboxModule,
    NgxSpinnerModule,
  ],
})
export class SharedModule {}
