import React from 'react'
import { inject, observer } from 'mobx-react'
import { createGlobalStyle } from 'styled-components'

const ThemeProvider = ({ commonStore, children }) => {

  const { solidColor, shadowColor, gradientColor, solidLightColor } = commonStore.appTheme

  const GlobalStyle = createGlobalStyle`
    html {
      --antd-wave-shadow-color: ${solidColor} !important;
    }
    ::selection {
      background: ${solidColor} !important;
    }
    .ant-input-number-input {
      font-size: 14px !important;
      font-weight: normal !important;
      &::placeholder {
        font-weight: normal !important;
      }
    }
    .ant-checkbox-checked {
      .ant-checkbox-inner {
        background-color: transparent !important;
        border-color: ${solidColor} !important;
        &:after {
          border-color: ${solidColor} !important;
        }
      }    
    }
    .ant-input-affix-wrapper:hover {
      .ant-input {
        &:not(.ant-input-disabled) {
          border-color: ${solidColor} !important;
        }
        &:focus, &:active {
          box-shadow: ${shadowColor} !important;
        }
      }
    }
    .ant-table {
      a {
        color: #666666 !important;
        &:hover {
          color: ${solidColor} !important;
        }
      }
    }
    .ant-checkbox-checked::after, .ant-checkbox:hover {
      border-color: ${solidColor} !important;
    }
    .ant-checkbox-wrapper:hover .ant-checkbox-inner, .ant-checkbox:hover .ant-checkbox-inner, .ant-checkbox-input:focus + .ant-checkbox-inner {
      border-color: ${solidColor} !important;
    }
    .ant-input:focus, .ant-input:active {
      border-color: ${solidColor} !important;
      outline: 0;
      box-shadow: ${shadowColor} !important;
    }
    .ant-input:hover {
      border-color: ${solidColor} !important;
      outline: 0;
    }
    .ant-calendar-selected-date {
      .ant-calendar-date {
        background-color: ${solidColor} !important;
        &:hover {
          background-color: ${solidColor} !important;
        }
      }
    }
    .ant-calendar-date:hover, .ant-calendar-range .ant-calendar-in-range-cell::before {
      background-color: ${solidLightColor} !important;
    }
    .ant-calendar-range .ant-calendar-selected-start-date .ant-calendar-date, .ant-calendar-range .ant-calendar-selected-end-date .ant-calendar-date {
      background-color: ${solidColor} !important;
    }
    .ant-calendar-today .ant-calendar-date {
      color: white !important;
      border-color: #666666 !important;
      background-color: #666666 !important;
      font-weight: normal !important;
      &:hover {
        background-color: #666666 !important;
      }
    }
    .color-link {
      color: ${solidColor} !important;
    }
    .color-svg {
      path {
        fill: ${solidColor} !important;
      }
    }
    .ant-btn {
      color: ${solidColor} !important;
      border-color: ${solidColor} !important;
      &:hover {
      border-color: white !important;
        box-shadow: ${shadowColor} !important;
      }
    }
    .ant-btn-primary {
      color: white !important;
      background: ${gradientColor} !important;
      border-color: ${solidColor} !important;
      &:hover {
        border-color: ${solidColor} !important;
        box-shadow: ${shadowColor} !important;
      }
      &[disabled] {
        opacity: .5;
        &:hover {
          box-shadow: none !important;
        }
      }
    }
    .custom-input-number {
      width: 100% !important;
      &:hover {
        border-color: ${solidColor} !important;
      }
      &.ant-input-number-focused {
        border-color: ${solidColor} !important;
        box-shadow: ${shadowColor} !important;
      }
    }
    .ant-switch-checked {
      background: ${gradientColor} !important;
    }
    .ant-pagination-item:focus a, .ant-pagination-item:hover a {
      color: ${solidColor} !important;
    }
    .ant-pagination-item {
      &:not(.ant-pagination-item-active) {
        a {
          transition: ease .3s;
          border-radius: 5px;
        }
        &:hover {
          a {
            background-color: ${solidLightColor} !important;
            transition: ease .3s;
          }
        }
      }
    }
    .ant-pagination-prev, .ant-pagination-next {
      color: #C6CACC !important;
      &:not(.ant-pagination-disabled) {
        &:hover {
          color: ${solidColor} !important;
          background-color: ${solidLightColor} !important;
        }
      }
    }
    .ant-calendar-today-btn, .ant-calendar-header a:hover  {
      color: ${solidColor} !important;
    }
    .ant-select-selection {
      &:hover {
        border-color: ${solidColor} !important;
      }
      &:focus {
        box-shadow: ${shadowColor} !important;
      }
    }
    .ant-select-open .ant-select-selection {
      border-color: ${solidColor} !important;
      box-shadow: ${shadowColor} !important;
    }
    .ant-select-dropdown-menu-item:hover:not(.ant-select-dropdown-menu-item-disabled),
    .ant-select-dropdown-menu-item-active:not(.ant-select-dropdown-menu-item-disabled) {
      background-color: ${solidLightColor} !important;
      font-weight: normal !important;
      color: ${solidColor} !important;
      .anticon {
        color: ${solidColor} !important;
      }
    }
    .ant-select-dropdown.ant-select-dropdown--multiple .ant-select-dropdown-menu-item-selected .ant-select-selected-icon, .ant-select-dropdown.ant-select-dropdown--multiple .ant-select-dropdown-menu-item-selected:hover .ant-select-selected-icon {
      color: ${solidColor} !important;
    }
    .ant-select-dropdown-menu-item-selected {
      font-weight: normal !important;
    }
    .ant-select-focused .ant-select-selection, .ant-select-selection:focus, .ant-select-selection:active {
      border-color: ${solidColor} !important;
      box-shadow: ${shadowColor} !important;
    }
    .ant-btn-danger {
      border-color: rgb(244, 67, 54) !important;
      color: white !important;
      &:hover {
        box-shadow: 0 2px 10px rgba(254, 81, 150, 0.5) !important;
        border-color: rgb(244, 67, 54) !important;
        background-color: rgb(244, 67, 54) !important;
      }
      &.ant-btn-background-ghost {
        background-color: white !important;
        color: rgb(244, 67, 54) !important;
        border: 1px solid #E3E5E5 !important;
        &:hover {
          box-shadow: none !important;
          border: 1px solid #E3E5E5 !important;
        }
      }
    }
    .ant-popover-buttons {
      > .ant-btn:first-child {
        border: 1px solid #e5e5e5 !important;
        color: rgb(244, 67, 54) !important;
        &:hover, &:active {
          border: 1px solid #e5e5e5 !important;
          box-shadow: none !important;
        }
      }
    }
    .ant-radio-checked .ant-radio-inner {
      border-color: ${solidColor} !important;
      &:after {
        background-color: ${solidColor} !important;
      }
    }
    .ant-time-picker-input {
      &:hover, &:focus {
        border-color: ${solidColor} !important;
      }
      &:focus {
        box-shadow: ${shadowColor} !important;
      }
    }
    .ant-tabs-nav .ant-tabs-tab-active {
      color: ${solidColor} !important;
      font-weight: normal !important;
    }
    .ant-tabs-ink-bar {
      background-color: ${solidColor} !important;
    }
    .ant-timeline-item-tail {
      border-left: 1px solid ${solidColor} !important;
    }
    .ant-timeline-item-head {
      color: ${solidColor} !important;
      border-color: ${solidColor} !important;
    }
    .ant-time-picker-panel-select li:hover {
      background-color: ${solidLightColor} !important;
    }
    .ant-time-picker-panel-select-option-selected {
      color: ${solidColor} !important;
    }
    li.ant-time-picker-panel-select-option-selected {
      background: ${solidLightColor} !important;
    }
    .ant-calendar-picker:hover .ant-calendar-picker-input:not(.ant-input-disabled) {
      border-color: ${solidColor} !important;
    }
    .ant-calendar-time .ant-calendar-footer .ant-calendar-time-picker-btn {
      color: ${solidColor} !important;
    }
    .ant-calendar .ant-calendar-ok-btn {
      &:not(.ant-calendar-ok-btn-disabled) {
        background: ${solidColor} !important;
        border-color: ${solidColor} !important;
      }
    }
    .ant-calendar-time-picker-select li:hover, li.ant-calendar-time-picker-select-option-selected {
      background: ${solidLightColor} !important;
    }
    li.ant-calendar-time-picker-select-option-selected {
      color: ${solidColor} !important;
    }
    .ant-calendar-time-picker-select li:focus {
      color: ${solidColor} !important;
    }
  `

  return (
    <React.Fragment>
      <GlobalStyle/>
      {children}
    </React.Fragment>
  )
}

export default inject('commonStore')(observer(ThemeProvider))