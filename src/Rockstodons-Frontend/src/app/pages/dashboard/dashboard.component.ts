import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less']
})
export class DashboardComponent implements OnInit {
  isCollapsed = false;
  currentYear = new Date().getFullYear();

  constructor() {

  }

  ngOnInit(): void {
    
  }

}
