<template>
    <div class="labeled_block">
        <div class="labeled_block-title">
            <slot name="title"></slot>
            <div v-if="lined" class="labeled_block-title_line"></div>
            <slot name="buttons"></slot>
        </div>
        <div class="labeled_block-content">
            <slot name="content"></slot>
        </div>
    </div>
</template>

<style>
.labeled_block-title {
    width: 100%;
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;
    align-items: center;
}
.labeled_block-title_line {
    background-color: #534590;
    flex: 1 1 100%;
    height: 1px;
    border: none;
}
</style>

<script lang="ts">
import Vue from 'vue';
export default Vue.extend({
    props: {
        lined: Boolean,
    },
});
</script>
