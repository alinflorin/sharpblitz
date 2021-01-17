import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { EditorComponent } from './editor/editor.component';


const routes: Routes = [
  { path: '', component: EditorComponent },
  { path: '**', pathMatch: 'full', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
