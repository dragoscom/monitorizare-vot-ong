import { ErrorIndicatorComponent } from './error-indicator/error-indicator.component';
import { LoadingIndicatorComponent } from './loading-indicator/loading-indicator.component';
import { PaginationComponent } from './pagination/pagination.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TabsModule, CollapseModule } from 'ng2-bootstrap';
@NgModule({
    imports: [FormsModule, CommonModule, CollapseModule, TabsModule, RouterModule],
    exports: [FormsModule, CommonModule, CollapseModule, TabsModule, RouterModule, PaginationComponent, LoadingIndicatorComponent, ErrorIndicatorComponent],
    declarations: [PaginationComponent, LoadingIndicatorComponent, ErrorIndicatorComponent],
    providers: []
})
export class SharedModule {

}