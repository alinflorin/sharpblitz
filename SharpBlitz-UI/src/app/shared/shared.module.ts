import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { AngularSplitModule } from 'angular-split';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTabsModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatSidenavModule,
    MatListModule,
    AngularSplitModule,
    MatInputModule
  ],
  exports: [
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTabsModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatSidenavModule,
    MatListModule,
    AngularSplitModule,
    MatInputModule
  ]
})
export class SharedModule { }
