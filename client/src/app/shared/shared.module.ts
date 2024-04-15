import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './paging-header/paging-header.component';
import { PagerComponent } from './pager/pager.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';
import { FooterComponent } from './footer/footer.component';
import {FormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    LoadingSpinnerComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    FormsModule
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    LoadingSpinnerComponent,
    FooterComponent
  ]
})
export class SharedModule { }
