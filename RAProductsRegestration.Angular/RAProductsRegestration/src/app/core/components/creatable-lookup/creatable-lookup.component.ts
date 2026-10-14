import {
  Component,
  EventEmitter,
  Input,
  Output
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';


export interface LookupItem {
  id: string;
  name: string;
}


@Component({
  selector: 'app-creatable-lookup',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './creatable-lookup.component.html',

  styleUrl:
    './creatable-lookup.component.css'
})
export class CreatableLookupComponent {

  private static nextId = 0;
  readonly inputId = `lookup-${CreatableLookupComponent.nextId++}`;
  activeIndex = -1;

  @Input()
  label = '';

  @Input()
  placeholder = 'Search...';

  @Input()
  items: LookupItem[] = [];

  @Input()
  selectedId: string | null = null;

  @Input()
  disabled = false;

  @Input()
  creating = false;


  @Output()
  selectedIdChange =
    new EventEmitter<string | null>();

  @Output()
  createRequested =
    new EventEmitter<string>();


  searchText = '';

  opened = false;


  get filteredItems(): LookupItem[] {

    const search =
      this.searchText
        .trim()
        .toLowerCase();

    if (!search) {
      return this.items;
    }

    return this.items.filter(
      item =>
        item.name
          .toLowerCase()
          .includes(search)
    );
  }


  get selectedItem(): LookupItem | null {

    return this.items.find(
      x => x.id === this.selectedId
    ) ?? null;
  }


  get exactMatchExists(): boolean {

    const search =
      this.searchText
        .trim()
        .toLowerCase();

    if (!search) {
      return false;
    }

    return this.items.some(
      x =>
        x.name
          .trim()
          .toLowerCase() === search
    );
  }


  get canCreate(): boolean {

    return (
      !!this.searchText.trim() &&
      !this.exactMatchExists
    );
  }


  focus(): void {

    if (this.disabled) {
      return;
    }

    this.opened = true;
    this.activeIndex = -1;

    /*
     * When opening, allow the user
     * to immediately type a new search.
     */
    this.searchText = '';
  }


  select(
    item: LookupItem
  ): void {

    this.selectedId =
      item.id;

    this.searchText =
      item.name;

    this.opened =
      false;

    this.selectedIdChange.emit(
      item.id
    );
  }


  create(): void {

    const name =
      this.searchText.trim();

    if (
      !name ||
      this.exactMatchExists ||
      this.creating
    ) {
      return;
    }

    this.createRequested.emit(
      name
    );
  }


  clear(): void {

    this.selectedId =
      null;

    this.searchText =
      '';

    this.selectedIdChange.emit(
      null
    );

    this.opened =
      true;
  }


  closeLater(): void {

    /*
     * Small delay allows option click
     * to happen before dropdown closes.
     */
    setTimeout(
      () => {
        this.opened = false;

        if (!this.selectedId) {
          this.searchText = '';
        }
      },
      150
    );
  }

  onSearch(value: string): void {
    this.searchText = value;
    this.opened = true;
    this.activeIndex = -1;
  }

  onKeydown(event: KeyboardEvent): void {
    if (this.disabled) return;

    if (event.key === 'Escape' || event.key === 'Tab') {
      this.opened = false;
      this.activeIndex = -1;
      return;
    }

    const items = this.filteredItems;
    const optionCount = items.length + (this.canCreate ? 1 : 0);
    if (event.key === 'ArrowDown' || event.key === 'ArrowUp') {
      event.preventDefault();
      this.opened = true;
      if (!optionCount) return;
      this.activeIndex = event.key === 'ArrowDown'
        ? (this.activeIndex + 1) % optionCount
        : (this.activeIndex <= 0 ? optionCount - 1 : this.activeIndex - 1);
      document.getElementById(`${this.inputId}-option-${this.activeIndex}`)
        ?.scrollIntoView({ block: 'nearest' });
    } else if (event.key === 'Enter' && this.opened && this.activeIndex >= 0) {
      event.preventDefault();
      if (this.activeIndex < items.length) this.select(items[this.activeIndex]);
      else if (this.canCreate) this.create();
    }
  }
}
