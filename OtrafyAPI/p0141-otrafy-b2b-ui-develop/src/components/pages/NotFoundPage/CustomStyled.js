import React from 'react'
import styled, { keyframes } from 'styled-components'

const blink = keyframes`
    to {
      visibility: hidden;
    }
  `

export const Wrapper = styled.section`
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    overflow: hidden;
    .content {
      width: 500px;
      height: 300px;
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      background-color: rgba(0, 0, 0, .75);
      padding: 45px 20px 20px;
      box-sizing: border-box;
      box-shadow: 0 0 25px rgba(0, 0, 0, .3);
      border-radius: 5px 5px 0 0;
    }
    
    .browser-bar {
      background: #f9f9f3;
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      padding: 5px;
      overflow: hidden;
      border-radius: 5px 5px 0 0;
    }
    
    .button {
      display: inline-block;
      float: left;
      border-radius: 50%;
      width: 15px;
      height: 15px;
      margin-right: 5px;
    }
    
    .close {
      background: #fc635d;
    }
    
    .min {
      background: #fdbc40;
    }
    
    .max {
      background: #34c84a;
    }
    
    .text {
      color: white;
      p {
        margin-bottom: 0;
        font-size: 13px;
        font-family: monospace, sans-serif !important;
        a {
          font-family: monospace, sans-serif !important;
          text-decoration: underline;
        }
      }
    }
    .indicator {
      display: inline-block;
      width: 10px;
      height: 20px;
      background-color: #606060;
      vertical-align: middle;
      animation: ${blink} 1.5s steps(2, start) infinite;
    }
  `