import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less']
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('genresManagementLink') genresManagementLink!: ElementRef<HTMLAnchorElement>;

  isCollapsed = false;
  currentYear = new Date().getFullYear();

  constructor() {

  }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {
    this.genresManagementLink.nativeElement.click();
  }

}
