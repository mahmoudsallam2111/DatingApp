import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { LightboxModule } from 'ngx-lightbox';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FileUploadModule } from 'ng2-file-upload';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from 'ngx-timeago';
import { TuiRootModule, TuiSvgModule } from '@taiga-ui/core';
import { TuiInputModule } from '@taiga-ui/kit';

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
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    ButtonsModule.forRoot(),
    TimeagoModule.forRoot(),
    FileUploadModule,
    TuiRootModule,
    TuiInputModule,
    TuiSvgModule,
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    LightboxModule,
    NgxSpinnerModule,
    BsDatepickerModule,
    PaginationModule,
    ButtonsModule,
    TimeagoModule,
    FileUploadModule,
    TuiRootModule,
    TuiInputModule,
    TuiSvgModule,
  ],
})
export class SharedModule {}
